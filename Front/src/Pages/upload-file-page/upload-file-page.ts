import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { interval, Subscription, switchMap, takeWhile } from 'rxjs';
import Swal from 'sweetalert2';
import { AnalyticsService } from '../../core/core-service';
@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upload-file-page.html',
})
export class UploadFileComponent {
  private AnalyticsService = inject(AnalyticsService);
  uploadStatus = signal<'idle' | 'uploading' | 'processing' | 'completed' | 'failed'>('idle');
  batchId = signal<string | null>(null);
  errorMessage = signal<string | null>(null);
  selectedFileName: string = '';
  private pollingSub?: Subscription;
  router = inject(Router);
  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName = file.name;
      this.startUpload(file);
    }
  }
  private startUpload(file: File): void {
    this.uploadStatus.set('uploading');
    this.errorMessage.set(null);
    this.AnalyticsService.uploadFile(file).subscribe({
      next: (response) => {
        this.batchId.set(response.data);
        this.startPolling(response.data);
      },
      error: (err) => {
        this.uploadStatus.set('failed');
        Swal.fire({
          icon: 'error',
          title: 'failed to upload',
          text: 'try again later',
          confirmButtonColor: '#1E40AF',
          confirmButtonText: 'okً',
        });
      },
    });
  }
  private startPolling(id: string): void {
    this.uploadStatus.set('processing');
    this.pollingSub = interval(2000)
      .pipe(
        switchMap(() => this.AnalyticsService.getUploadStatus(id)),
        takeWhile((res) => res.data.status !== 'Completed' && res.data.status !== 'Failed', true),
      )
      .subscribe({
        next: (res) => {
          if (res.data.status === 'Completed') {
            this.uploadStatus.set('completed');
            this.stopPolling();
            Swal.fire({
              icon: 'success',
              title: 'success',
              text: 'data processed successfully',
              timer: 3000,
              timerProgressBar: true,
            }).then((result) => {
              this.router.navigate(['/dashboard']);
            });
          } else if (res.data.status === 'Failed') {
            this.handleProcessingError();
          }
        },
        error: () => this.handleProcessingError(),
      });
  }
  private stopPolling(): void {
    this.pollingSub?.unsubscribe();
  }
  ngOnDestroy(): void {
    this.stopPolling();
  }
  private handleProcessingError(): void {
    this.uploadStatus.set('failed');
    this.stopPolling();
    Swal.fire({
      icon: 'error',
      title: 'error in processing data',
      text: 'try again later',
      confirmButtonColor: '#1E40AF',
    });
  }
}

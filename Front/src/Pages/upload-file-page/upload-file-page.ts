import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
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
  private pollingSub?: Subscription;
  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
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
        // تنبيه المستخدم بفشل الرفع
        Swal.fire({
          icon: 'error',
          title: 'فشل الرفع',
          text: 'حدث خطأ تقني أثناء إرسال الملف للسيرفر، يرجى المحاولة لاحقاً.',
          confirmButtonColor: '#1E40AF', // لون الـ Primary اللي اخترناه
          confirmButtonText: 'حسناً',
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
              title: 'تمت المعالجة',
              text: 'تم تحليل البيانات المالية ورفعها بنجاح!',
              timer: 3000,
              showConfirmButton: false,
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
      title: 'خطأ في معالجة البيانات',
      text: 'تم رفع الملف ولكن فشلنا في قراءة محتواه، تأكد من مطابقة الملف للمعايير المطلوبة.',
      confirmButtonColor: '#1E40AF',
    });
  }
}

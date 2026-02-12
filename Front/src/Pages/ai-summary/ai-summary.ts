import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { AnalyticsService } from '../../core/core-service';

@Component({
  selector: 'app-ai-insights',
  templateUrl: './ai-summary.html',
  imports: [DatePipe],
})
export class AiInsightsComponent implements OnInit {
  insights: string[] = [];
  tips: string[] = [];
  generatedAt: Date | null = null;
  isLoading = true;
  router = inject(Router);
  constructor(
    private aiService: AnalyticsService,
    private cdr: ChangeDetectorRef,
  ) {}
  ngOnInit(): void {
    const start = new Date('2026-01-01');
    const end = new Date('2026-02-01');
    this.isLoading = true;
    this.aiService.getAIInsights(start, end).subscribe({
      next: (res) => {
        if (!res?.data) {
          this.showErrorAlert('Received empty data from the server.');
          return;
        }
        const text = res.data.analysisText;
        this.generatedAt = new Date(res.data.generatedAt);
        const sections = text.split('Tips to save money:');
        this.insights = sections[0].match(/\* (.*)/g)?.map((s) => s.replace('* ', '')) || [];
        this.tips = sections[1]?.match(/\* (.*)/g)?.map((s) => s.replace('* ', '')) || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.showErrorAlert('Connection error: Unable to reach the AI Service.');
      },
    });
  }
  private showErrorAlert(message: string): void {
    Swal.fire({
      title: 'Oops!',
      text: message,
      icon: 'error',
      confirmButtonColor: '#4F46E5',
      confirmButtonText: 'Try Again',
    });
  }
  navigateToUpload() {
    this.router.navigate(['/upload'], { replaceUrl: true });
  }
}

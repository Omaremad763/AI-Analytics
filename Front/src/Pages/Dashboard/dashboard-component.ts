import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import Chart from 'chart.js/auto';
import { NgApexchartsModule } from 'ng-apexcharts';
import { AnalyticsService } from '../../core/core-service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, NgApexchartsModule],
  templateUrl: './dashboard-component.html',
})
export class DashboardComponent implements OnInit {
  metrics: any[] = [];
  categoryData: any[] = [];
  trendData: any;
  processedCategories: any[] = [];
  private trendChart: Chart | undefined;
  private doughnutChart: Chart | undefined;

  @ViewChild('trendCanvas') trendCanvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild('doughnutCanvas') doughnutCanvas!: ElementRef<HTMLCanvasElement>;

  constructor(
    private analyticsService: AnalyticsService,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData() {
    const start = new Date('2026-01-01');
    const end = new Date('2026-02-01');

    this.analyticsService.getmetrics().subscribe((res) => {
      if (res.success) {
        this.metrics = res.data;
        this.cdr.detectChanges();
      }
    });

    this.analyticsService.getcharts(start, end).subscribe((res) => {
      if (res.success) {
        this.trendData = res.data.trendData;
        this.categoryData = res.data.categoryDistribution;
        this.prepareTrendStats();
        this.setupFinancialCharts();
        this.cdr.detectChanges();
      }
    });
  }
  navigateToSummary() {
    this.router.navigate(['/aisummary']);
  }

  navigateToUpload() {
    this.router.navigate(['/upload'], { replaceUrl: true });
  }
  totalAmount: number = 0;
  topCategoryName: string = '';
  private prepareTrendStats() {
    if (!this.categoryData || this.categoryData.length === 0) return;
    this.totalAmount = this.categoryData.reduce((acc, curr) => acc + curr.totalAmount, 0);
    const topCat = [...this.categoryData].sort((a, b) => b.totalAmount - a.totalAmount)[0];
    this.topCategoryName = topCat.categoryName;
  }
  chartOptions: any;
  private setupFinancialCharts() {
    const series = this.categoryData.map((c) => c.totalAmount);
    const labels = this.categoryData.map((c) => c.categoryName);
    this.chartOptions = {
      series: this.categoryData.map((c) => c.totalAmount),
      chart: {
        type: 'donut',
        height: 280,
        sparkline: { enabled: true },
      },
      labels: this.categoryData.map((c) => c.categoryName),
      colors: ['#0f0268', '#eaff00', '#ec4899', '#f43f5e'],
      plotOptions: {
        pie: {
          donut: {
            size: '85%',
          },
        },
      },
      dataLabels: { enabled: false },
    };
  }
}

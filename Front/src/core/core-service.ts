import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environment';
import { ApiResponse } from '../shared/api-response.model';
import * as models from './core-models';

@Injectable({
  providedIn: 'root',
})
export class AnalyticsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;
  //Data Ingestion
  uploadFile(file: File): Observable<ApiResponse<string>> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}/DataIngetion/upload`, formData);
  }
  // get upload status
  getUploadStatus(id: string): Observable<ApiResponse<models.UploadStatusDto>> {
    return this.http.get<ApiResponse<models.UploadStatusDto>>(
      `${this.baseUrl}/DataIngetion/status/${id}`,
    );
  }
  // dashboard
  getDashboardData(start: Date, end: Date): Observable<ApiResponse<models.AnalyticsDashboardDto>> {
    const params = new HttpParams().set('start', start.toISOString()).set('end', end.toISOString());
    return this.http.get<ApiResponse<models.AnalyticsDashboardDto>>(
      `${this.baseUrl}/Dashboard/dashboard`,
      { params },
    );
  }
  getBatchDetails(id: string): Observable<ApiResponse<models.FinancialRecordDto[]>> {
    return this.http.get<ApiResponse<models.FinancialRecordDto[]>>(
      `${this.baseUrl}/Dashboard/batch/${id}`,
    );
  }
  // AI insights
  getAIInsights(startDate: Date, endDate: Date): Observable<ApiResponse<models.AIInsightDto>> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());
    return this.http.get<ApiResponse<models.AIInsightDto>>(`${this.baseUrl}/AIInsights/analyze`, {
      params,
    });
  }
}

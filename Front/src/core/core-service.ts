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
  getmetrics(): Observable<ApiResponse<models.MetricCardDto[]>> {
    return this.http.get<ApiResponse<models.MetricCardDto[]>>(`${this.baseUrl}/Dashboard/metrics`);
  }
  getcharts(start: Date, end: Date): Observable<ApiResponse<any>> {
    const s = start.toISOString().split('T')[0];
    const e = end.toISOString().split('T')[0];
    const params = new HttpParams().set('start', s).set('end', e);
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/Dashboard/charts`, { params });
  }
  // AI insights
  getAIInsights(start: Date, end: Date): Observable<ApiResponse<models.AIInsightDto>> {
    const params = new HttpParams().set('start', start.toISOString()).set('end', end.toISOString());
    return this.http.get<ApiResponse<models.AIInsightDto>>(`${this.baseUrl}/AIInsights/analyze`, {
      params,
    });
  }
}

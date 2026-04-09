import { Routes } from '@angular/router';
import { AiInsightsComponent } from '../Pages/ai-summary/ai-summary';
import { DashboardComponent } from '../Pages/Dashboard/dashboard-component';
import { UploadFileComponent } from '../Pages/upload-file-page/upload-file-page';
export const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'upload', component: UploadFileComponent },
  { path: 'aisummary', component: AiInsightsComponent },
  { path: '', component: UploadFileComponent },
];

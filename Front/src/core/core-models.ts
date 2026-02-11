export interface AIInsightDto {
  analysisText: string;
  generatedAt: string;
  periodStart: string;
  periodEnd: string;
}
export interface DailyTransactionSummaryDto {
  date: string;
  totalAmount: number;
  transactionCount: number;
}
export interface AnalyticsDashboardDto {
  totalIncome: number;
  totalExpense: number;
  netProfit: number;
  dailySummaries: DailyTransactionSummaryDto[];
}
export interface FinancialRecordDto {
  category: string;
  amount: number;
  transactionDate: string;
  description: string;
}
export interface UploadStatusDto {
  id: string;
  fileName: string;
  status: string;
  errorMessage?: string; // ? معناها optional
  uploadDate: Date;
  processedAt?: Date; // optional
}

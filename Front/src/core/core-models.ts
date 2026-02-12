export interface AIInsightDto {
  analysisText: string;
  generatedAt: string;
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
  errorMessage?: string;
  uploadDate: Date;
  processedAt?: Date;
}
export interface MetricCardDto {
  label: string;
  value: number;
  icon: string;
  color: string;
}

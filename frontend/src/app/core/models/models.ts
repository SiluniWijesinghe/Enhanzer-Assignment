export interface LoginRequest {
  email: string;
  password: string;
}

export interface UserLocation {
  Location_Code: string;
  Location_Name: string;
}

export interface LoginResponse {
  token: string;
  expiresAtUtc: string;
  email: string;
  locations: UserLocation[];
}

export interface PurchaseBillLine {
  item: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  qty: number;
  freeQty: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}

export interface PurchaseBillResponse {
  purchaseBillId: number;
  lines: PurchaseBillLine[];
  totalItems: number;
  totalQuantity: number;
  totalCostSum: number;
  totalSellingSum: number;
  createdAtUtc: string;
}

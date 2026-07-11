import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PurchaseBillLine, PurchaseBillResponse, UserLocation } from '../models/models';

@Injectable({ providedIn: 'root' })
export class PurchaseBillService {
  constructor(private http: HttpClient) {}

  getItems(): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiBaseUrl}/purchasebill/items`);
  }

  getLocations(): Observable<UserLocation[]> {
    return this.http.get<UserLocation[]>(`${environment.apiBaseUrl}/locations`);
  }

  submit(lines: PurchaseBillLine[]): Observable<PurchaseBillResponse> {
    return this.http.post<PurchaseBillResponse>(`${environment.apiBaseUrl}/purchasebill`, { lines });
  }
}

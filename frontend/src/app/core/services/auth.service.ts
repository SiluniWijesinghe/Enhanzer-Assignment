import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse, UserLocation } from '../models/models';

const TOKEN_KEY = 'enhanzer_token';
const EXPIRY_KEY = 'enhanzer_token_expiry';
const EMAIL_KEY = 'enhanzer_email';
const LOCATIONS_KEY = 'enhanzer_locations';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenSignal = signal<string | null>(sessionStorage.getItem(TOKEN_KEY));

  isAuthenticated = computed(() => {
    const token = this.tokenSignal();
    if (!token) return false;
    return !this.isExpired();
  });

  constructor(private http: HttpClient) { }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiBaseUrl}/auth/login`, request).pipe(
      tap((response) => {
        sessionStorage.setItem(TOKEN_KEY, response.token);
        sessionStorage.setItem(EXPIRY_KEY, response.expiresAtUtc);
        sessionStorage.setItem(EMAIL_KEY, response.email);
        sessionStorage.setItem(LOCATIONS_KEY, JSON.stringify(response.locations));
        this.tokenSignal.set(response.token);
      })
    );
  }

  logout(): void {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(EXPIRY_KEY);
    sessionStorage.removeItem(EMAIL_KEY);
    sessionStorage.removeItem(LOCATIONS_KEY);
    this.tokenSignal.set(null);
  }

  getToken(): string | null {
    return this.tokenSignal();
  }

  getEmail(): string | null {
    return sessionStorage.getItem(EMAIL_KEY);
  }

  getCachedLocations(): UserLocation[] {
    const raw = sessionStorage.getItem(LOCATIONS_KEY);
    return raw ? JSON.parse(raw) : [];
  }

  private isExpired(): boolean {
    const expiry = sessionStorage.getItem(EXPIRY_KEY);
    if (!expiry) return true;
    return new Date(expiry).getTime() <= Date.now();
  }
}

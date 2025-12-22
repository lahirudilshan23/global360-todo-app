import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthResponse, LoginRequest } from '../models/auth.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = environment.apiBaseUrl + 'api/auth';

  constructor(private http: HttpClient) {}

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      request
    );
  }

  storeTokens(response: AuthResponse): void {
    localStorage.setItem('accessToken', response.accessToken.token);
    localStorage.setItem('accessTokenExpiresAt', response.accessToken.expiresAt);
    localStorage.setItem('refreshToken', response.refreshToken.token);
  }

  get accessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  logout(): void {
    localStorage.clear();
  }

  isLoggedIn(): boolean {
    return !!this.accessToken;
  }

  refreshToken() {
    const token = localStorage.getItem('refreshToken');
    if (!token) return;

    return this.http.post<AuthResponse>(
        `${this.apiUrl}/refresh`,
        { refreshToken: token }
    );
  }
}
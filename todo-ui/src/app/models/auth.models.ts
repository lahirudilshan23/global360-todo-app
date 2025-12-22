export interface AuthToken {
  token: string;
  expiresAt: string;
}

export interface AuthResponse {
  accessToken: AuthToken;
  refreshToken: AuthToken;
}

export interface LoginRequest {
  username: string;
  password: string;
}
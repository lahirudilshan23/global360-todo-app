import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../models/auth.models';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    MatToolbarModule, 
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(
        private authService: AuthService, 
        private router: Router, 
        private snackBar: MatSnackBar
    ) {}

  login() {
  if (!this.username || !this.password) {
    return; // If username or password is empty, do nothing
  }

  const request: LoginRequest = {
    username: this.username,
    password: this.password
  };

  this.authService.login(request).subscribe({
    next: (res) => {
      this.authService.storeTokens(res);
      this.router.navigate(['/todos']);
    },
    error: (err) => {
      // Handle network or backend errors
      console.error(err);

        if (err.status === 401) {
            this.snackBar.open('Username or password is incorrect', 'Close', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['snackbar-error']
        });
        } else {
            this.snackBar.open('Something went wrong. Please try again.', 'Close', {
            duration: 3000
        });
        }
    }
  });
 }
} 
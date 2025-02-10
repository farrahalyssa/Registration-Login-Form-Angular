import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms'; 

interface User {
  email: string;
  userPassword: string;
  isActive: true;
}
@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent {
  user: User = { email: '', userPassword: '', isActive: true };

  constructor(private authService: AuthService, private router: Router) {}

  loginUser() {
    this.authService.login(this.user).subscribe(
      response => {
        console.log('User logged in:', response);
        if (response && response.token) {
          this.authService.saveToken(response.token);
          alert('Login successful!');
          this.router.navigate(['/home']); 
        } else {
          alert('Login failed. No token received.');
        }
      },
      error => {
        console.error('Login error:', error);
        alert('Invalid email or password.');
      }
    );
  }
}

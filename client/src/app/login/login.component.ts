import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms'; 

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent {
  user = { userEmail: '', userPassword: '' };

  constructor(private authService: AuthService, private router: Router) {}

  loginUser() {
    this.authService.login(this.user).subscribe(
      response => {
        console.log('User logged in:', response);
        this.authService.saveToken(response.token); // Save JWT token
        alert('Login successful!');
        this.router.navigate(['/home']); 
      },
      error => {
        console.error('Login error:', error);
        alert('Invalid email or password.');
      }
    );
  }
}

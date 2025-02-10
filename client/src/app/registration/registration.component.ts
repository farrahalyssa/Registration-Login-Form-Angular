import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { v4 as uuidv4 } from 'uuid';
import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
interface User {
  userName: string;
  email: string;
  userPassword: string;
  userId: string;
  isActive: boolean;
}

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  user: User = { userName: '', userId: uuidv4(), email: '', userPassword: '', isActive: true };
  isLoading = false;  // To handle loading state
  errorMessage = '';  // To display error messages
  private http = inject(HttpClient);

  constructor(private authService: AuthService, private router: Router) {}
  
  registerUser() {
    this.isLoading = true;  // Set loading state to true
    this.authService.register(this.user).subscribe(
      response => {
        console.log('User registered successfully:', response);
        this.router.navigate(['/home']);
      },
      error => {
        console.error('Registration error:', error);
        
      },
      () => {
        this.isLoading = false;  // Reset loading state
      }
    );
  }
}

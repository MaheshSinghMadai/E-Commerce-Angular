import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [
    MatButton
  ],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss'
})
export class TestErrorComponent {
  baseUrl = 'https://localhost:7201';
  validationErrors?: string[];

  private http = inject(HttpClient);

  get404Error() {
    this.http.get(this.baseUrl + '/Buggy/NotFound').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400Error() {
    this.http.get(this.baseUrl + '/Buggy/BadRequest').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get401Error() {
    this.http.get(this.baseUrl + '/Buggy/Unauthorized').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500Error() {
    this.http.get(this.baseUrl + '/Buggy/InternalError').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400ValidationError() {
    this.http.post(this.baseUrl + '/Buggy/ValidationError', {}).subscribe({
      next: response => console.log(response),
      error: error => this.validationErrors = error
    })
  }
}

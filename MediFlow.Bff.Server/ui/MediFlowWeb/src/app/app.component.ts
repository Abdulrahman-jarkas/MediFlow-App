import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  private readonly httpClient = inject(HttpClient);

  title = 'MediFlowWeb';

  ngOnInit(): void {
    this.httpClient.get<any>(`${this.getCurrentHost()}/api/User`).subscribe();
  }

  getClinics() {
    this.httpClient
      .get<any>(`${this.getCurrentHost()}/bff/api/clinics`)
      .subscribe();
  }

  // why this not working
  logout() {
    this.httpClient
      .post(`${this.getCurrentHost()}/api/Account/Logout`, {})
      .subscribe();
  }

  // why this cause CORS origin
  login() {
    this.httpClient
      .get<any>(`${this.getCurrentHost()}/api/Account/Login`)
      .subscribe();
  }

  createClinic() {
    this.httpClient
      .post(`${this.getCurrentHost()}/bff/api/clinics`, {
        name: 'C1',
        rooms: [
          {
            name: 'C1R1',
          },
        ],
        doctors: [
          {
            name: 'C1D1',
          },
        ],
        medicines: [
          {
            name: 'C1M1',
            price: 10,
            quantity: 40,
          },
        ],
        equipment: [
          {
            name: 'C1E1',
            fee: 10,
          },
        ],
      })
      .subscribe();
  }

  updateClinic() {
    this.httpClient
      .put(
        `${this.getCurrentHost()}/bff/api/clinics/fe3f730f-948e-4f45-a52e-c9ec60ce950d`,
        {
          name: 'C1 - update',
          userId: '76a21138-068d-406f-a2ed-7b4daea024f0',
          rooms: [
            {
              name: 'C1R1  - update',
              id: 'accb29f3-b33b-4d7c-9d1e-c5664187b351',
            },
          ],
          doctors: [
            {
              name: 'C1D1  - update',
              id: 'd6bd8947-0281-441a-9183-24e41a60061c',
            },
          ],
          equipment: [
            {
              name: 'C1E1  - update',
              fees: 10,
              id: 'fd301e6d-34d4-4992-ba72-3f916394b2ca',
            },
          ],
          medicines: [
            {
              name: 'C1M1  - update',
              price: 10,
              quantity: 40,
              id: '1426d3e5-9eb5-49ae-8616-5956344b33b5',
            },
          ],
          id: 'fe3f730f-948e-4f45-a52e-c9ec60ce950d',
        }
      )
      .subscribe();
  }

  private getCurrentHost() {
    const host = window.location.host;
    const url = `${window.location.protocol}//${host}`;

    return url;
  }
}

import { Component, inject } from '@angular/core';
import { ControlService } from '../../Services/control.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { login } from '../../Interfaces/login';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    CommonModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  private accesoService = inject(ControlService);
  private router = inject(Router);
  public formBuild = inject(FormBuilder);

  public formLogin: FormGroup = this.formBuild.group({
    usuario: ['', Validators.required],
    pass: ['', Validators.required]
  });

  iniciarSesion() {
    if (this.formLogin.invalid) return;

    const objeto: login = {
      usuario: this.formLogin.value.usuario,
      pass: this.formLogin.value.pass
    };

    this.accesoService.login(objeto).subscribe({
      next: (data) => {
        if (data.isSuccess) {
          localStorage.setItem("token", data.token);
          this.router.navigate(['inicio']);
        } else {
          alert("Credenciales son incorrectas");
        }
      },
      error: (error) => {
        console.log(error.message);
      }
    });
  }

  registrarse() {
    this.router.navigate(['registro']);
  }
}

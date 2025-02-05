import { Component, inject } from '@angular/core';
import { ControlService } from '../../Services/control.service';
import { Router } from '@angular/router';
import { usuario } from '../../Interfaces/usuario';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, CommonModule, FormsModule], 
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent {

  private accesoService = inject(ControlService);
  private router = inject(Router);

  usuario: string = '';
  pass: string = '';

  registrarse(form: any) {
    if (form.invalid) return;

    const objeto: usuario = {
      usuario: this.usuario,
      pass: this.pass
    };

    this.accesoService.registrarse(objeto).subscribe({
      next: (data) => {
        if (data.isSuccess) {
          this.router.navigate(['']);
        } else {
          alert("No se pudo registrar");
        }
      },
      error: (error) => {
        console.log(error.message);
      }
    });
  }

  volver() {
    this.router.navigate(['']);
  }
}

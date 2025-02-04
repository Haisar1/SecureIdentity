import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ControlService } from '../../Services/control.service';
import { Router } from '@angular/router';
import { usuario } from '../../Interfaces/usuario';

import {MatCardModule} from '@angular/material/card';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [MatCardModule,MatFormFieldModule,MatInputModule,MatButtonModule,CommonModule],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent {

     private accesoService = inject(ControlService);
     private router = inject(Router);
     public formBuild = inject(FormBuilder);

     public formRegistro: FormGroup = this.formBuild.group({
          usuario: ['',Validators.required],
          pass: ['',Validators.required]
     })

     registrarse(){
          if(this.formRegistro.invalid) return;

          const objeto:usuario = {
               usuario: this.formRegistro.value.usuario,
               pass: this.formRegistro.value.pass,
              }

          this.accesoService.registrarse(objeto).subscribe({
               next: (data) =>{
                    if(data.isSuccess){
                         this.router.navigate([''])
                    }else{
                         alert("No se pudo registrar")
                    }
               }, error:(error) =>{
                    console.log(error.message);
               }
          })

     }

     volver(){
          this.router.navigate([''])
     }

}
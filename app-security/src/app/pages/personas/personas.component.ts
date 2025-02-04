import { Component, OnInit } from '@angular/core';
import { Persona } from '../../Interfaces/persona';
import { PersonaService } from '../../Services/persona.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-personas',
  templateUrl: './personas.component.html',
  styleUrls: ['./personas.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class PersonasComponent implements OnInit {

  listPersonas: Persona[] = [];
  Formulario: FormGroup;

  constructor(
    private personaService: PersonaService, 
    private formBuilder: FormBuilder 
  ) {
    this.Formulario = this.formBuilder.group({
      nombres: [null, Validators.required],
      apellidos: [null, Validators.required],
      numeroIdentificacion: [null, Validators.required],
      email: [null, Validators.required],
      tipoIdentificacion: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    this.obtenerPersonas();
  }

  obtenerPersonas() {
    this.personaService.getPersonas().subscribe({
      next: (data) => {
        this.listPersonas = data;
      },
      error: (error) => {}
    });
  }

  agregarPersona() {
    const request: Persona = {
      identificador: 0,
      nombres: this.Formulario.value.nombres,
      apellidos: this.Formulario.value.apellidos,
      numeroIdentificacion: this.Formulario.value.numeroIdentificacion,
      email: this.Formulario.value.email,
      tipoIdentificacion: this.Formulario.value.tipoIdentificacion,
    };

    this.personaService.add(request).subscribe({
      next: (data) => {
        this.listPersonas.push(data);
        this.Formulario.patchValue({
          nombres: '',
          apellidos: '',
          numeroIdentificacion: '',
          email: '',
          tipoIdentificacion: '',
        });
      },
      error: (error) => {}
    });
  }

  eliminarPersona(persona: Persona) {
    this.personaService.delete(persona.identificador).subscribe({
      next: (data) => {
        const newlist = this.listPersonas.filter(x => x.identificador !== persona.identificador);
        this.listPersonas = newlist;
      },
      error: (error) => {}
    });
  }
}

import { Component, OnInit, inject } from '@angular/core';
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

  Formulario: FormGroup;
  private personaService = inject(PersonaService);
  listPersonas: Persona[] = [];
  personaSeleccionada: Persona | null = null;
  showErrorPopup: boolean = false;
  showEditPopup: boolean = false;

  constructor(private formBuilder: FormBuilder) {
    this.Formulario = this.formBuilder.group({
      nombres: [null, Validators.required],
      apellidos: [null, Validators.required],
      numeroIdentificacion: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      tipoIdentificacion: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    this.obtenerPersonas();
  }
   mostrarErrorPopup() {
    this.showErrorPopup = true;
  }

  cerrarErrorPopup() {
    this.showErrorPopup = false;
  }

  obtenerPersonas() {
    this.personaService.getPersonas().subscribe({
      next: (data) => {
        console.log(data);
        if (Array.isArray(data) && data.length > 0) {
          this.listPersonas = data;
        }
      },
      error: (error) => { 
        this.mostrarErrorPopup();  
       }
    });
  }

  agregarPersona() {
    const request: Persona = this.Formulario.value;
    request.identificador = 0;
    
    this.personaService.add(request).subscribe({
      next: (data) => {
        this.listPersonas.push(data);
        this.Formulario.reset();
      },
      error: (error) => {
        this.mostrarErrorPopup(); }
    });
  }

  eliminarPersona(persona: Persona) {
    this.personaService.delete(persona.identificador).subscribe({
      next: () => {
        this.listPersonas = this.listPersonas.filter(x => x.identificador !== persona.identificador);
      },
      error: (error) => { console.error(error); }
    });
  }

  seleccionarPersona(persona: Persona) {
    this.personaSeleccionada = persona;
    this.Formulario.reset();
    this.Formulario.patchValue(persona);
    this.showEditPopup = true;
  }
  
  actualizarPersona() {
    if (!this.personaSeleccionada) return;
    const request: Persona = { ...this.personaSeleccionada, ...this.Formulario.value };
    
    this.personaService.update(this.personaSeleccionada.identificador, request).subscribe({
      next: (data) => {
        const index = this.listPersonas.findIndex(p => p.identificador === data.identificador);
        if (index !== -1) {
          this.listPersonas[index] = data;
        }
        this.Formulario.reset();
        this.personaSeleccionada = null;
        this.showEditPopup = false;
      },
      error: (error) => { this.mostrarErrorPopup(); }
    });
  }

  cerrarPopup() {
    this.showEditPopup = false;
    this.Formulario.reset();
  }
}

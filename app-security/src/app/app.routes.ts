import { Routes } from '@angular/router';
import { LoginComponent } from '../app/pages/login/login.component';
import { RegistroComponent } from './pages/registro/registro.component';
import { PersonasComponent } from './pages/personas/personas.component';
import { authGuard } from './custom/auth.guard';

export const routes: Routes = [
     {path:"", component:LoginComponent},
     {path:"registro", component:RegistroComponent},
     {path:"personas", component:PersonasComponent , canActivate:[authGuard]},
];
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ControlService } from '../Services/control.service';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = () => {
     const token = localStorage.getItem("token") || "";
     const router = inject(Router);

     const accesoService = inject(ControlService)
     if(token != ""){
          return accesoService.validarToken(token).pipe(
               map(data => {
                    if(data.isSuccess){
                         return true
                    } else{
                         router.navigate([''])
                         return false;
                    }
               }),
               catchError(error => {
                    router.navigate([''])
                         return of(false);
               })
          )
     }else {
          const url = router.createUrlTree([""])
          return url;
     }
  
};
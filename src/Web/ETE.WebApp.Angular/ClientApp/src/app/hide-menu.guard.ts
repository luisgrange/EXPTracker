import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class HideMenuGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Lógica para decidir se o menu deve ser ocultado
    const isLoginPage = state.url === '/login';

    if (isLoginPage) {
      // Oculte o menu
      return true; // O menu é ocultado na página de login
    } else {
      // Mostre o menu em todas as outras páginas
      return true; // Ou você pode retornar false para bloquear o acesso a outras páginas
    }
  }
}

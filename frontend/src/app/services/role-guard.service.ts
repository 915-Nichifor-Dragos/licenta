import { Injectable, inject } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  UrlTree,
  Router,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../features/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  authService = inject(AuthService);
  router = inject(Router);

  canActivate(
    next: ActivatedRouteSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const expectedRoles = next.data['expectedRoles'];

    const roleChecks = expectedRoles.map((role: string | null) => {
      if (role === null) {
        return false;
      } else {
        return this.authService.hasRole(role);
      }
    });

    return Promise.all(roleChecks)
      .then((results) => {
        const hasAtLeastOneRole = results.some((result) => result === true);

        if (hasAtLeastOneRole) {
          return true;
        } else {
          this.router.navigate(['/auth/login']); // Navigate to unauthorized or any other appropriate route (update when adding 403)

          return false;
        }
      })
      .catch((error) => {
        console.error('Error occurred while checking role:', error);

        return false;
      });
  }
}

import { TestBed, inject } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { authGuard } from './auth.guard';

describe('authGuard', () => {
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(() => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ]
    });
  });

  it('should allow access if user is authenticated', () => {
    mockAuthService.isAuthenticated.and.returnValue(true);

    const result = TestBed.runInInjectionContext(() =>
      authGuard({} as any, {} as any)
    );

    expect(result).toBeTrue();
  });

  it('should redirect to /auth if not authenticated', () => {
    mockAuthService.isAuthenticated.and.returnValue(false);
    mockRouter.navigate.and.returnValue(Promise.resolve(true));

    const result = TestBed.runInInjectionContext(() =>
      authGuard({} as any, {} as any)
    );

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/auth']);
    expect(result).toEqual(mockRouter.navigate(['/auth']));
  });
});

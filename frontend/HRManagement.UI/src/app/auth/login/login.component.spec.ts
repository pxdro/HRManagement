import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { LoginComponent } from './login.component';
import { AuthService } from 'src/app/core/services/auth.service';
import { SharedModule } from 'src/app/shared/shared.module';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['login']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [ReactiveFormsModule, SharedModule, BrowserAnimationsModule, NoopAnimationsModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have invalid form when empty', () => {
    expect(component.loginForm.valid).toBeFalse();
  });

  it('should login and navigate on success', () => {
    component.loginForm.setValue({ email: 'test@example.com', password: '123456' });
    mockAuthService.login.and.returnValue(of({data: { authToken: 'fake-token' }, errorMessage: ''}));

    component.onSubmit();

    expect(mockAuthService.login).toHaveBeenCalledWith('test@example.com', '123456');
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/employees']);
    expect(component.loginError).toBeFalse();
  });

  it('should set loginError to true on failed login', () => {
    component.loginForm.setValue({ email: 'test@example.com', password: 'wrongpass' });
    mockAuthService.login.and.returnValue(throwError(() => new Error('Unauthorized')));

    component.onSubmit();

    expect(mockAuthService.login).toHaveBeenCalled();
    expect(mockRouter.navigate).not.toHaveBeenCalled();
    expect(component.loginError).toBeTrue();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { TopnavComponent } from './topnav.component';
import { SharedModule } from '../../shared.module';

describe('TopnavComponent', () => {
  let component: TopnavComponent;
  let fixture: ComponentFixture<TopnavComponent>;
  let authServiceMock: jasmine.SpyObj<AuthService>;
  let routerMock: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authSpy = jasmine.createSpyObj('AuthService', ['getUserEmail', 'logout']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [TopnavComponent],
      providers: [
        { provide: AuthService, useValue: authSpy },
        { provide: Router, useValue: routerSpy }
      ],
      imports: [ SharedModule ]
    }).compileComponents();

    authServiceMock = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    routerMock = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TopnavComponent);
    component = fixture.componentInstance;
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should get user email on initialization', () => {
    authServiceMock.getUserEmail.and.returnValue('test@example.com');
    
    fixture.detectChanges();
    
    expect(component.userEmail).toBe('test@example.com');
    expect(authServiceMock.getUserEmail).toHaveBeenCalled();
  });

  it('should navigate to employees page', () => {
    component.redirectEmployees();
    expect(routerMock.navigate).toHaveBeenCalledWith(['/employees']);
  });

  it('should navigate to departments page', () => {
    component.redirectDepartments();
    expect(routerMock.navigate).toHaveBeenCalledWith(['/departments']);
  });

  it('should logout and navigate to login page', () => {
    component.logout();
    
    expect(authServiceMock.logout).toHaveBeenCalled();
    
    expect(routerMock.navigate).toHaveBeenCalledWith(['/login']);
  });
});
import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse, Tokens } from 'src/app/shared/models/apiModels.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login', () => {
    let mockResponse: ApiResponse<Tokens> = { data: { authToken: 'test-token' }, errorMessage: '' };
    
    service.login('test@example.com', 'password').subscribe(response => {
      expect(response).toEqual(mockResponse);
      expect(localStorage.getItem('authToken')).toBe('test-token');
    });

    const req = httpMock.expectOne('api/auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });
});

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DepartmentService } from './department.service';
import { ApiResponse, Department } from 'src/app/shared/models/apiModels.model';

describe('DepartmentService', () => {
  let service: DepartmentService;
  let httpMock: HttpTestingController;
  const mockDepartment = { id: '463a0204-e70c-40aa-a16c-30a9aea7b226', name: 'HR', description: '' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(DepartmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get departments', () => {
    service.getDepartments().subscribe();

    const req = httpMock.expectOne('api/department');
    expect(req.request.method).toBe('GET');
  });

  it('should get single department', () => {
    service.getDepartment('463a0204-e70c-40aa-a16c-30a9aea7b226').subscribe();

    const req = httpMock.expectOne('api/department/463a0204-e70c-40aa-a16c-30a9aea7b226');
    expect(req.request.method).toBe('GET');
  });

  it('should add department', () => {
    service.addDepartment(mockDepartment).subscribe();

    const req = httpMock.expectOne('api/department');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockDepartment);
  });

  it('should update department', () => {
    const updatedDepartment: Department = {
      ...mockDepartment,
      name: 'Updated HR Department'
    };

    const mockResponse: ApiResponse<Department> = {
      data: updatedDepartment,
      errorMessage: ''
    };

    service.updateDepartment(updatedDepartment).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/${updatedDepartment.id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedDepartment);
    req.flush(mockResponse);
  });

  it('should delete department', () => {
    const departmentId = '463a0204-e70c-40aa-a16c-30a9aea7b226';
    const mockResponse: ApiResponse<boolean> = {
      data: true,
      errorMessage: ''
    };

    service.deleteDepartment(departmentId).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/${departmentId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockResponse);
  });
});
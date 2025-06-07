import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EmployeeService } from './employee.service';
import { ApiResponse, Employee } from 'src/app/shared/models/apiModels.model';

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;
  const mockEmployee = { 
    id: '463a0204-e70c-40aa-a16c-30a9aea7b226', 
    name: 'HR', 
    email: 'test@email.com',
    password: 'HashedPass123',
    position: 'Software Engineer',
    hireDate: new Date(2023,1,1),
    isAdmin: false,
    departmentId: '44b4f1c3-39db-4fc1-a3d3-99f11636c027',
    department: { 
      id: '44b4f1c3-39db-4fc1-a3d3-99f11636c027',
      name: 'IT',
      description: ''
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get employees', () => {
    service.getEmployees().subscribe();

    const req = httpMock.expectOne('api/employee');
    expect(req.request.method).toBe('GET');
  });

  it('should get single employee', () => {
    service.getEmployee('463a0204-e70c-40aa-a16c-30a9aea7b226').subscribe();

    const req = httpMock.expectOne('api/employee/463a0204-e70c-40aa-a16c-30a9aea7b226');
    expect(req.request.method).toBe('GET');
  });

  it('should add employee', () => {
    service.addEmployee(mockEmployee).subscribe();

    const req = httpMock.expectOne('api/employee');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockEmployee);
  });

  it('should update employee', () => {
    const updatedEmployee: Employee = {
      ...mockEmployee,
      name: 'Updated HR Employee'
    };

    const mockResponse: ApiResponse<Employee> = {
      data: updatedEmployee,
      errorMessage: ''
    };

    service.updateEmployee(updatedEmployee).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/${updatedEmployee.id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedEmployee);
    req.flush(mockResponse);
  });

  it('should delete employee', () => {
    const employeeId = '463a0204-e70c-40aa-a16c-30a9aea7b226';
    const mockResponse: ApiResponse<boolean> = {
      data: true,
      errorMessage: ''
    };

    service.deleteEmployee(employeeId).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/${employeeId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockResponse);
  });
});
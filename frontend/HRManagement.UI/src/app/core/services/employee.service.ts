import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, Employee } from 'src/app/shared/models/apiModels.model';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private apiUrl = 'api/employee';

  constructor(private http: HttpClient) {}

  getEmployees(): Observable<ApiResponse<Employee[]>> {
    return this.http.get<ApiResponse<Employee[]>>(this.apiUrl);
  }

  getEmployee(id: string): Observable<ApiResponse<Employee>> {
    return this.http.get<ApiResponse<Employee>>(`${this.apiUrl}/${id}`);
  }

  addEmployee(employee: Employee): Observable<ApiResponse<Employee>> {
    return this.http.post<ApiResponse<Employee>>(this.apiUrl, employee);
  }

  updateEmployee(employee: Employee): Observable<ApiResponse<Employee>> {
    return this.http.put<ApiResponse<Employee>>(`${this.apiUrl}/${employee.id}`, employee);
  }

  deleteEmployee(id: string): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/${id}`);
  }
}
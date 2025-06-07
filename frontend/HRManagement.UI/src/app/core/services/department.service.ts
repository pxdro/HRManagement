import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, Department } from 'src/app/shared/models/apiModels.model';

@Injectable({ providedIn: 'root' })
export class DepartmentService {
  private apiUrl = 'api/department';

  constructor(private http: HttpClient) {}

  getDepartments(): Observable<ApiResponse<Department[]>> {
    return this.http.get<ApiResponse<Department[]>>(this.apiUrl);
  }

  getDepartment(id: string): Observable<ApiResponse<Department>> {
    return this.http.get<ApiResponse<Department>>(`${this.apiUrl}/${id}`);
  }

  addDepartment(Department: Department): Observable<ApiResponse<Department>> {
    return this.http.post<ApiResponse<Department>>(this.apiUrl, Department);
  }

  updateDepartment(Department: Department): Observable<ApiResponse<Department>> {
    return this.http.put<ApiResponse<Department>>(`${this.apiUrl}/${Department.id}`, Department);
  }

  deleteDepartment(id: string): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/${id}`);
  }
}
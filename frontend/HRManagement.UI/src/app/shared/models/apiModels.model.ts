export interface Tokens {
  authToken: string;
}

export interface Department {
  id: string;
  name: string;
  description: string;
}

export interface Employee {
  id: string;
  name: string;
  email: string;
  password: string;
  position: string;
  hireDate: Date;
  isAdmin: boolean;
  departmentId: string;
  department: Department;
}

export interface ApiResponse<T> {
  errorMessage: string;
  data: T;
}

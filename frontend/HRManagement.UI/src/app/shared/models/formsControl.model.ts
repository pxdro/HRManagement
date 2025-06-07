import { FormControl } from "@angular/forms";

export interface EmployeeFormControls {
  name: FormControl;
  email: FormControl;
  password: FormControl;
  position: FormControl;
  hireDate: FormControl;
  isAdmin: FormControl;
  departmentId: FormControl;
}

export interface DepartmentFormControls {
  name: FormControl;
  description: FormControl;
}
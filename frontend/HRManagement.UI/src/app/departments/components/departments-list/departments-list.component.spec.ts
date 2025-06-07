import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DepartmentsListComponent } from './departments-list.component';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { selectAllDepartments, selectDepartmentLoading } from '../../state/department.selectors';
import { loadDepartments, deleteDepartment } from '../../state/department.actions';
import { of } from 'rxjs';
import { By } from '@angular/platform-browser';
import { SharedModule } from 'src/app/shared/shared.module';

describe('DepartmentsListComponent', () => {
  let component: DepartmentsListComponent;
  let fixture: ComponentFixture<DepartmentsListComponent>;
  let store: MockStore;
  let router: Router;

  const mockDepartments = [
    { id: 'b04eefcf-3da9-499e-b0e9-9b2d70d5a715', name: 'IT' },
    { id: 'e6493599-3f42-477f-b27f-f9063cbe80e1', name: 'HR' }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DepartmentsListComponent],
      providers: [
        provideMockStore({
          selectors: [
            { selector: selectAllDepartments, value: mockDepartments },
            { selector: selectDepartmentLoading, value: false }
          ]
        }),
        { provide: Router, useValue: { navigate: jasmine.createSpy('navigate') } }
      ],
      imports: [SharedModule]
    }).compileComponents();

    store = TestBed.inject(Store) as MockStore;
    router = TestBed.inject(Router);
    spyOn(store, 'dispatch');
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch loadDepartments on init', () => {
    expect(store.dispatch).toHaveBeenCalledWith(loadDepartments());
  });

  it('should display departments list', () => {
    const departmentElements = fixture.debugElement.queryAll(By.css('.department-card'));
    expect(departmentElements.length).toBe(mockDepartments.length);
  });

  it('should navigate to add department page', () => {
    component.onAdd();
    expect(router.navigate).toHaveBeenCalledWith(['/departments/new']);
  });

  it('should navigate to edit department page', () => {
    const departmentId = 'b04eefcf-3da9-499e-b0e9-9b2d70d5a715';
    component.onEdit(departmentId);
    expect(router.navigate).toHaveBeenCalledWith([`/departments/${departmentId}/edit`]);
  });

  describe('onDelete', () => {
    beforeEach(() => {
      (store.dispatch as jasmine.Spy).calls.reset();
    });

    it('should dispatch deleteDepartment when confirmed', () => {
      spyOn(window, 'confirm').and.returnValue(true);
      const departmentId = 'b04eefcf-3da9-499e-b0e9-9b2d70d5a715';
      const departmentName = 'IT';
      
      component.onDelete(departmentName, departmentId);
      
      expect(window.confirm).toHaveBeenCalledWith(`Are you sure you want to delete the department ${departmentName}?`);
      expect(store.dispatch).toHaveBeenCalledWith(deleteDepartment({ id: departmentId }));
    });

    it('should not dispatch deleteDepartment when not confirmed', () => {
      spyOn(window, 'confirm').and.returnValue(false);
      const departmentId = 'b04eefcf-3da9-499e-b0e9-9b2d70d5a715';
      const departmentName = 'IT';
      
      component.onDelete(departmentName, departmentId);
      
      expect(window.confirm).toHaveBeenCalledWith(`Are you sure you want to delete the department ${departmentName}?`);
      expect(store.dispatch).not.toHaveBeenCalled();
    });
  });

  it('should show loading indicator when loading', () => {
    store.overrideSelector(selectDepartmentLoading, true);
    store.refreshState();
    fixture.detectChanges();
    
    const loadingElement = fixture.debugElement.query(By.css('.loading'));
    expect(loadingElement).toBeTruthy();
  });
});
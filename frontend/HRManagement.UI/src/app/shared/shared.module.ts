import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopnavComponent } from './components/topnav/topnav.component';

// Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatToolbarModule } from '@angular/material/toolbar';

@NgModule({
  declarations: [
    TopnavComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatCheckboxModule,
    MatToolbarModule,
  ],
  exports: [
    TopnavComponent,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatCheckboxModule,
    MatToolbarModule,
  ]
})
export class SharedModule { }

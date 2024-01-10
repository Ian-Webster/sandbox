import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from './nav/nav/nav.component';

@NgModule({
  imports: [
    CommonModule,
	NavComponent
  ],
  exports: [
    CommonModule
  ]
})
export class SharedModule { }
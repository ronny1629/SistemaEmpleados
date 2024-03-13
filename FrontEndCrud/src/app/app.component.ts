import { Component } from '@angular/core';
import {AfterViewInit, ViewChild, OnInit} from '@angular/core';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { Empleado } from './Interface/empleado';
import { EmpleadoService } from './Services/empleado.service';

import { AddEditComponent } from './Dialog/add-edit/add-edit.component';

import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements AfterViewInit, OnInit{
  displayedColumns: string[] = ['NombreCompleto', 'Departamento', 'Sueldo', 'FechaContrato','Acciones'];
  dataSource = new MatTableDataSource<Empleado>();

  constructor(private _empleadoServicio:EmpleadoService,public dialog: MatDialog){

  }



  ngOnInit(): void {
      this.mostrarEmpleados();
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;


  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  mostrarEmpleados(){
    this._empleadoServicio.getList().subscribe({
      next:(dataResponse) =>{
        console.log(dataResponse)
        this.dataSource.data = dataResponse;
      },error:(e)=>{}
    })
  }


  dialogoNuevoEmpleado() {
    this.dialog.open(AddEditComponent, {
      disableClose: true,
      width: "350px",

    }).afterClosed().subscribe(resultado =>{
      if (resultado == "creado") {
        this.mostrarEmpleados();
      }
    })
  }

  dialogoEditarEmpleado(dataEmpleado: Empleado){
    this.dialog.open(AddEditComponent, {
      disableClose: true,
      width: "350px",
      data:dataEmpleado

    }).afterClosed().subscribe(resultado =>{
      if (resultado == "editado") {
        this.mostrarEmpleados();
      }
    })
  }
}



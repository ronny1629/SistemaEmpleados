import { Component, Inject, OnInit} from '@angular/core';
import { FormBuilder,FormGroup,Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import moment, { Moment } from 'moment';
import { Departamento } from '../../Interface/departamento';
import { Empleado } from '../../Interface/empleado';
import { DepartamentoService } from '../../Services/departamento.service';
import { EmpleadoService } from '../../Services/empleado.service';



export const My_DATE_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY'
  },
  display:{
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY'
  }
}


@Component({
  selector: 'app-add-edit',
  templateUrl: './add-edit.component.html',
  styleUrl: './add-edit.component.css',
  providers: [
    {provide: MAT_DATE_FORMATS, useValue: My_DATE_FORMATS}
  ]
})


  export class AddEditComponent implements OnInit{

    formEmpleado: FormGroup;
    tituloAccion: string = "Nuevo";
    botonAccion: string = "Guardar";
    listaDepartamentos: Departamento[] = [];


    constructor(
       private dialogoReferencia: MatDialogRef<AddEditComponent>,
       private fb:FormBuilder,
       private _snackBar:MatSnackBar,
       private _departamentoServicio: DepartamentoService,
       private _empleadoServicio: EmpleadoService,
       @Inject(MAT_DIALOG_DATA) public dataEmpleado: Empleado

      ){

        this.formEmpleado = this.fb.group({
          nombreCompleto:["", Validators.required],
          idDepartamento:["", Validators.required],
          sueldo:["", Validators.required],
          fechaContrato:["", Validators.required]
        })

        this._departamentoServicio.getList().subscribe({
          next:(data) => {
            this.listaDepartamentos = data;
          },error:(e) => {}
        })

      }

      mostrarAlerta(msg: string, accion: string) {
        this._snackBar.open(msg, accion,{
          horizontalPosition:"end",
          verticalPosition:"top",
          duration: 2300
        });
      }


      addEditEmpleado(){



        const modelo : Empleado = {
          idEmpleado: 0,
          nombreCompleto: this.formEmpleado.value.nombreCompleto,
          idDepartamento: this.formEmpleado.value.idDepartamento,
          sueldo: this.formEmpleado.value.sueldo,
          fechaContrato: moment(this.formEmpleado.value.fechaContrato).format("DD/MM/YYYY")
        }

        if(this.dataEmpleado == null){

          this._empleadoServicio.add(modelo).subscribe({
            next: (data) => {
              this.mostrarAlerta("Empleado Creado exitosamente!!", "Listo");
              this.dialogoReferencia.close("creado");
            },error:(e)=>{
              this.mostrarAlerta("No se pudo crear el usuario!!", "Error");
            }
          })

        }else{
          this._empleadoServicio.update(this.dataEmpleado.idEmpleado, modelo).subscribe({
            next: (data) => {
              this.mostrarAlerta("Empleado Actualizado exitosamente!!", "Listo");
              this.dialogoReferencia.close("editado");
            },error:(e)=>{
              this.mostrarAlerta("No se pudo editar el usuario!!", "Error");
            }
          })
        }


      }

    ngOnInit(): void {

      if (this.dataEmpleado) {
        this.formEmpleado.patchValue({
          nombreCompleto: this.dataEmpleado.nombreCompleto,
          idDepartamento:this.dataEmpleado.idDepartamento,
          sueldo:this.dataEmpleado.sueldo,
          fechaContrato:moment(this.dataEmpleado.fechaContrato,"DD/MM/YYYY")
        })

        this.tituloAccion = "Editar";
        this.botonAccion = "Actualizar";
      }
    }

  }








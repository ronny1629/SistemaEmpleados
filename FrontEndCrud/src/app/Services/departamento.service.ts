import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { Observable } from 'rxjs';
import { Departamento } from '../Interface/departamento';

@Injectable({
  providedIn: 'root'
})
export class DepartamentoService {
  private endPoint:string = environment.endPoint;
  private apiUrl:string = this.endPoint + "departamento/"


  constructor(private http:HttpClient) {}

    getList():Observable<Departamento[]>{
      return this.http.get<Departamento[]>(`${this.apiUrl}lista`)
    }

}

import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface DialogBebeuMuitoData {
  mensagem: string;
}

@Component({
  selector: 'app-dialog-bebeu-muito',
  templateUrl: './dialog-bebeu-muito.component.html',
  styleUrls: ['./dialog-bebeu-muito.component.scss']
})
export class DialogBebeuMuitoComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<DialogBebeuMuitoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogBebeuMuitoData
  ) {}

  ngOnInit(): void {
  }

  sair(): void {
    this.dialogRef.close();
  }

}

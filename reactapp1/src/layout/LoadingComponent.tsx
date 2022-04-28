
import React from "react";
import { Spinner } from "react-bootstrap";

interface Props{
    message:string;
}

export default function LoadingComponent({message}: Props) {
    const style = { position: "fixed" as "fixed", top: "50%", left: "50%", transform: "translate(-50%, -50%)" };
    return (
        <div style={style}>
            <Spinner animation="border" role="status">

            </Spinner>
            <br/>
            <span className="font-weight-bold">{message}</span>
        </div>
    )
}
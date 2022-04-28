import { useField } from 'formik';
import { FormGroup, FormLabel } from 'react-bootstrap';

interface Props {
    placeholder: string;
    name: string;
    label: string;
    type?: string;
}

export default function MyTextInput(props: Props) {
    const [field, meta] = useField(props.name);
    return (

        <FormGroup className='mb-3'>
            <FormLabel>{props.label}</FormLabel>
            <input {...field} placeholder={props.placeholder} type={props.type}  className={'form-control' + (meta.error && meta.touched ? ' is-invalid' : '')} />

            {meta.touched && meta.error ? (
                <div className="invalid-feedback" >{meta.error}</div>
            ) : null}

        </FormGroup> 
    )
}


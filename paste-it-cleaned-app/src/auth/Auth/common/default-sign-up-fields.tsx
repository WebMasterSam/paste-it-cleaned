export interface ISignUpField {
    label: string
    key: string
    placeholder: string
    required: boolean
    displayOrder: number
    invalid?: boolean
    custom?: boolean
    type?: string
}

export const signUpWithEmailFields: ISignUpField[] = [
    {
        label: 'Email',
        key: 'email',
        required: true,
        placeholder: 'Email',
        type: 'email',
        displayOrder: 1,
    },
    {
        label: 'Password',
        key: 'password',
        required: true,
        placeholder: 'Password',
        type: 'password',
        displayOrder: 2,
    },
    {
        label: 'First name',
        key: 'name',
        placeholder: 'First name',
        required: true,
        displayOrder: 3,
    },
    {
        label: 'Last name',
        key: 'family_name',
        placeholder: 'Last name',
        required: true,
        displayOrder: 4,
    },
    {
        label: 'Phone Number',
        key: 'phone_number',
        placeholder: 'Phone Number',
        required: true,
        displayOrder: 5,
    },
]

export const signUpWithPhoneNumberFields: ISignUpField[] = [
    {
        label: 'Phone Number',
        key: 'phone_number',
        placeholder: 'Phone Number',
        required: true,
        displayOrder: 1,
    },
    {
        label: 'Password',
        key: 'password',
        required: true,
        placeholder: 'Password',
        type: 'password',
        displayOrder: 2,
    },
    {
        label: 'Email',
        key: 'email',
        required: true,
        placeholder: 'Email',
        type: 'email',
        displayOrder: 3,
    },
]

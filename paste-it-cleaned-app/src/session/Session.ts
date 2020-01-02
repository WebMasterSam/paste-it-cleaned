class Session {
    constructor(isValid: boolean, userName: string, firstName: string, lastName: string, email: string, culture: string) {
        this.IsValid = isValid
        this.UserName = userName
        this.FirstName = firstName
        this.LastName = lastName
        this.Email = email
        this.Culture = culture
    }

    readonly IsValid: boolean = false
    readonly UserName: string = ''
    readonly FirstName: string = ''
    readonly LastName: string = ''
    readonly Email: string = ''
    readonly Culture: string = 'en-US'
}

export var CurrentSession = new Session(false, '', '', '', '', 'en-US')

export function replaceCurrentSession(isValid: boolean, userName: string, firstName: string, lastName: string, email: string, culture: string) {
    CurrentSession = new Session(isValid, userName, firstName, lastName, email, culture)
}

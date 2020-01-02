import * as React from 'react'

import { I18n, ConsoleLogger as Logger } from '@aws-amplify/core'
import Auth from '@aws-amplify/auth'

import AuthPiece, { IAuthPieceProps, IAuthPieceState } from './AuthPiece'
import { FormSection, SectionHeader, SectionBody, SectionFooter, FormField, Input, InputLabel, Button, Link, SectionFooterPrimaryContent, SectionFooterSecondaryContent } from '../Amplify-UI/Amplify-UI-Components-React'

import { auth } from '../Amplify-UI/data-test-attributes'

import countryDialCodes from './common/country-dial-codes'
import { signUpWithEmailFields, ISignUpField } from './common/default-sign-up-fields'
import { PhoneField } from './PhoneField'

const logger = new Logger('SignUp')

export interface ISignUpConfig {
    defaultCountryCode?: number | string
    header?: string
    hideAllDefaults?: boolean
    hiddenDefaults?: string[]
    signUpFields?: ISignUpField[]
}

export interface ISignUpProps extends IAuthPieceProps {
    signUpConfig?: ISignUpConfig
}

export default class SignUp extends AuthPiece<ISignUpProps, IAuthPieceState> {
    public defaultSignUpFields: ISignUpField[]
    public header: string
    public signUpFields: ISignUpField[] = []

    constructor(props: ISignUpProps) {
        super(props)
        this.state = { requestPending: false }

        this._validAuthStates = ['signUp']
        this.signUp = this.signUp.bind(this)
        this.sortFields = this.sortFields.bind(this)
        this.getDefaultDialCode = this.getDefaultDialCode.bind(this)
        this.needPrefix = this.needPrefix.bind(this)
        this.header = this.props && this.props.signUpConfig && this.props.signUpConfig.header ? this.props.signUpConfig.header : 'Create a new account'

        this.defaultSignUpFields = signUpWithEmailFields
    }

    validate() {
        const invalids: any[] = []
        this.signUpFields.map(el => {
            if (el.key !== 'phone_number') {
                if (el.required && !this.inputs[el.key]) {
                    el.invalid = true
                    invalids.push(el.label)
                } else {
                    el.invalid = false
                }
            } else {
                if (el.required && !this.phone_number) {
                    el.invalid = true
                    invalids.push(el.label)
                } else {
                    el.invalid = false
                }
            }
        })
        return invalids
    }

    sortFields() {
        if (this.props.signUpConfig && this.props.signUpConfig.hiddenDefaults && this.props.signUpConfig.hiddenDefaults.length > 0) {
            this.defaultSignUpFields = this.defaultSignUpFields.filter(d => {
                return !this.props!.signUpConfig!.hiddenDefaults!.includes(d.key)
            })
        }

        this.signUpFields = this.defaultSignUpFields
    }

    needPrefix(key: any) {
        const field = this.signUpFields.find(e => e.key === key)
        if (key.indexOf('custom:') !== 0) {
            return field!.custom
        } else if (key.indexOf('custom:') === 0 && field!.custom === false) {
            logger.warn('Custom prefix prepended to key but custom field flag is set to false; retaining manually entered prefix')
        }
        return null
    }

    getDefaultDialCode() {
        return this.props.signUpConfig && this.props.signUpConfig.defaultCountryCode && countryDialCodes.indexOf(`+${this.props.signUpConfig.defaultCountryCode}`) !== -1 ? `+${this.props.signUpConfig.defaultCountryCode}` : '+1'
    }

    signUp() {
        this.setState({ requestPending: true })
        if (!this.inputs.dial_code) {
            this.inputs.dial_code = this.getDefaultDialCode()
        }
        const validation = this.validate()
        if (validation && validation.length > 0) {
            this.setState({ requestPending: false })
            return this.error(`The following fields need to be filled out: ${validation.join(', ')}`)
        }
        if (!Auth || typeof Auth.signUp !== 'function') {
            this.setState({ requestPending: false })
            throw new Error('No Auth module found, please ensure @aws-amplify/auth is imported')
        }

        const signup_info = {
            username: this.inputs.username,
            password: this.inputs.password,
            attributes: {},
        }

        const inputKeys = Object.keys(this.inputs)
        const inputVals = Object.values(this.inputs)

        inputKeys.forEach((key, index) => {
            if (!['username', 'password', 'checkedValue', 'dial_code'].includes(key)) {
                if (key !== 'phone_line_number' && key !== 'dial_code' && key !== 'error') {
                    const newKey = `${this.needPrefix(key) ? 'custom:' : ''}${key}`
                    // @ts-ignore
                    signup_info.attributes[newKey] = inputVals[index]
                }
            }
        })

        if (this.phone_number)
            // @ts-ignore
            signup_info.attributes['phone_number'] = this.phone_number

        let labelCheck = false
        this.signUpFields.forEach(field => {
            if (field.label === this.getUsernameLabel()) {
                logger.debug(`Changing the username to the value of ${field.label}`)
                // @ts-ignore
                signup_info.username = signup_info.attributes[field.key] || signup_info.username
                labelCheck = true
            }
        })
        if (!labelCheck && !signup_info.username) {
            // if the customer customized the username field in the sign up form
            // He needs to either set the key of that field to 'username'
            // Or make the label of the field the same as the 'usernameAttributes'
            this.setState({ requestPending: false })
            throw new Error(`Couldn't find the label: ${this.getUsernameLabel()}, in sign up fields according to usernameAttributes!`)
        }
        Auth.signUp(signup_info)
            .then((data: any) => {
                this.setState({ requestPending: false })
                // @ts-ignore
                this.changeState('confirmSignUp', data.user.username)
            })
            .catch((err: any) => {
                this.setState({ requestPending: false })
                return this.error(err)
            })
    }

    showComponent(theme: any): React.ReactNode {
        const { hide } = this.props
        if (hide && hide.includes(SignUp)) {
            return null
        }
        this.sortFields()
        return (
            <FormSection theme={theme} data-test={auth.signUp.section} className="auth-form">
                <SectionHeader theme={theme} data-test={auth.signUp.headerSection}>
                    {I18n.get(this.header)}
                </SectionHeader>
                <SectionBody theme={theme} data-test={auth.signUp.bodySection}>
                    {this.signUpFields.map(field => {
                        return field.key !== 'phone_number' ? (
                            <FormField theme={theme} key={field.key}>
                                {field.required ? <InputLabel theme={theme}>{I18n.get(field.label)} *</InputLabel> : <InputLabel theme={theme}>{I18n.get(field.label)}</InputLabel>}
                                <Input
                                    autoFocus={
                                        this.signUpFields.findIndex(f => {
                                            return f.key === field.key
                                        }) === 0
                                            ? true
                                            : false
                                    }
                                    placeholder={I18n.get(field.placeholder)}
                                    theme={theme}
                                    type={field.type}
                                    name={field.key}
                                    key={field.key}
                                    onChange={this.handleInputChange}
                                    data-test={auth.signUp.nonPhoneNumberInput}
                                />
                            </FormField>
                        ) : (
                            <PhoneField theme={theme} required={field.required} defaultDialCode={this.getDefaultDialCode()} label={field.label} placeholder={field.placeholder} onChangeText={this.onPhoneNumberChanged} key="phone_number" />
                        )
                    })}
                </SectionBody>
                <SectionFooter theme={theme} data-test={auth.signUp.footerSection}>
                    <SectionFooterPrimaryContent theme={theme}>
                        <Button disabled={this.state.requestPending} onClick={this.signUp} theme={theme} data-test={auth.signUp.createAccountButton}>
                            {I18n.get('Create Account')}
                        </Button>
                    </SectionFooterPrimaryContent>
                    <SectionFooterSecondaryContent theme={theme}>
                        {I18n.get('Have an account? ')}
                        <Link theme={theme} onClick={() => this.changeState('signIn')} data-test={auth.signUp.signInLink}>
                            {I18n.get('Sign in')}
                        </Link>
                    </SectionFooterSecondaryContent>
                </SectionFooter>
            </FormSection>
        )
    }
}

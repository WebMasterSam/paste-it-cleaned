import React from 'react'

import { TextField, Paper, Grid, Button, Typography, Select, MenuItem, InputLabel, FormControl, Table, TableHead, TableRow, TableBody, TableCell, TablePagination, FormGroup, FormControlLabel } from '@material-ui/core'

import CreditCardIcon from '@material-ui/icons/CreditCard'
import HistoryIcon from '@material-ui/icons/History'

import FormWrapper from '../../../components/forms/FormWrapper'

import { VisaIcon } from '../../../icons/Visa'
import { MasterCardIcon } from '../../../icons/MasterCard'
import { AmexIcon } from '../../../icons/Amex'
import { PayPalIcon } from '../../../icons/PayPal'

import BillingTable from './components/BillingTable'
import { InvoiceEntity } from '../../../entities/api'

import './BillingInformation.less'
import { BillingController } from './BillingController'
import EmptyState from '../../../components/forms/EmptyState'

export interface BillingInformationProps {}
export interface BillingInformationState {
    isLoaded: boolean
    invoices: InvoiceEntity[]
    invoicesLoading: boolean
    invoicesError: boolean
    isChangingMethod: boolean
    isAddingPayPal: boolean
    isAddingStripe: boolean
}

class BillingInformation extends React.Component<BillingInformationProps, BillingInformationState> {
    private controller?: BillingController = undefined

    constructor(props: BillingInformationProps) {
        super(props)
        this.controller = new BillingController(this)
        this.state = {
            isLoaded: false,
            invoices: [],
            invoicesLoading: false,
            invoicesError: false,
            isChangingMethod: false,
            isAddingPayPal: false,
            isAddingStripe: false,
        }
    }

    componentDidMount() {
        this.controller!.initialize()
    }

    render() {
        return (
            <React.Fragment>
                <Grid container spacing={3}>
                    <Grid item xs={12} lg={4}>
                        {!this.state.isChangingMethod && !this.state.isAddingStripe && !this.state.isAddingPayPal && (
                            <Paper className="paper wide">
                                <Typography variant="h2" className="override-h2" component="h2">
                                    <CreditCardIcon /> Plan and payment method
                                </Typography>
                                <br />
                                <FormWrapper>
                                    <Grid container spacing={3}>
                                        <Grid item xs={4}>
                                            <Typography variant="body1" component="span">
                                                <b>Current plan</b>
                                            </Typography>
                                        </Grid>
                                        <Grid item xs={8}>
                                            <Typography variant="body1" component="span">
                                                Pay-as-you-go
                                            </Typography>
                                        </Grid>
                                    </Grid>
                                    <Grid container spacing={3}>
                                        <Grid item xs={4}>
                                            <Typography variant="body1" component="span">
                                                <b>Method</b>
                                            </Typography>
                                        </Grid>
                                        <Grid item xs={8}>
                                            <Typography variant="body1" component="span" className="override-body1">
                                                <PayPalIcon height="20px" /> samuelrb@dotmedias.com
                                            </Typography>
                                        </Grid>
                                    </Grid>
                                </FormWrapper>

                                <Grid container spacing={3}>
                                    <Grid item xs={12}>
                                        <a href="#" onClick={this.controller!.showChangePaymentMethod}>
                                            Update payment method
                                        </a>
                                    </Grid>
                                </Grid>
                            </Paper>
                        )}

                        {this.state.isChangingMethod && (
                            <Paper className="paper wide">
                                <Typography variant="h2" className="override-h2" component="h2">
                                    <CreditCardIcon /> Change method
                                </Typography>
                                <p>Please select the desired payment method :</p>
                                <Button variant="outlined" style={{ float: 'left' }} onClick={this.controller!.showChangePaymentMethodToStripe}>
                                    <VisaIcon height={50} style={{ marginLeft: '10px' }} />
                                    <MasterCardIcon height={50} style={{ marginLeft: '10px' }} />
                                    <AmexIcon height={50} style={{ marginLeft: '10px' }} />
                                </Button>
                                <Button variant="outlined" style={{ float: 'right' }} onClick={this.controller!.showChangePaymentMethodToPayPal}>
                                    <PayPalIcon height={50} />
                                </Button>
                            </Paper>
                        )}

                        {this.state.isAddingPayPal && <p>PayPal</p>}

                        {this.state.isAddingStripe && (
                            <Paper className="paper wide">
                                <Typography variant="h2" className="override-h2" component="h2">
                                    <CreditCardIcon /> Credit card
                                </Typography>

                                <FormWrapper>
                                    <Grid container spacing={3}>
                                        <Grid item xs={8}>
                                            <TextField name="owner" label="Name on card" fullWidth />
                                        </Grid>
                                        <Grid item xs={4}>
                                            <TextField name="cvv" label="CVV" fullWidth />
                                        </Grid>
                                    </Grid>

                                    <Grid container spacing={3}>
                                        <Grid item xs={12}>
                                            <TextField name="card-number" label="Card number" fullWidth />
                                        </Grid>
                                    </Grid>

                                    <Grid container spacing={3}>
                                        <Grid item xs={3}>
                                            <FormControl fullWidth>
                                                <InputLabel id="expiration-month-label">Month</InputLabel>
                                                <Select labelId="expiration-month-label" id="expiration-month" name="expiration-month" fullWidth>
                                                    <MenuItem value={'jan'}>January</MenuItem>
                                                </Select>
                                            </FormControl>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <FormControl fullWidth>
                                                <InputLabel id="expiration-year-label">Year</InputLabel>
                                                <Select labelId="expiration-year-label" id="expiration-year" name="expiration-year" fullWidth>
                                                    <MenuItem value={'2019'}>2019</MenuItem>
                                                </Select>
                                            </FormControl>
                                        </Grid>
                                        <Grid item xs={7} className="align-right">
                                            <VisaIcon height={55} style={{ marginLeft: '10px' }} />
                                            <MasterCardIcon height={55} style={{ marginLeft: '10px' }} />
                                            <AmexIcon height={55} style={{ marginLeft: '10px' }} />
                                        </Grid>
                                    </Grid>
                                </FormWrapper>

                                <Grid container spacing={3}>
                                    <Grid item xs={4}>
                                        <Button variant="contained" type="submit" color="primary" disabled={false}>
                                            Update
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Paper>
                        )}
                    </Grid>
                    <Grid item xs={12} lg={8}>
                        <Paper className="paper wide">
                            <Typography variant="h2" className="override-h2" component="h2">
                                <HistoryIcon /> Billing history
                            </Typography>

                            <FormWrapper>
                                <BillingTable loading={this.state.invoicesLoading} error={this.state.invoicesError} rows={this.state.invoices} full={true} />
                            </FormWrapper>
                        </Paper>
                    </Grid>
                </Grid>
            </React.Fragment>
        )
    }
}

export default BillingInformation

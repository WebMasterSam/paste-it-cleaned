import React from 'react'

import { TextField, Paper, Grid, Button, Typography, Select, MenuItem, InputLabel, FormControl, Table, TableHead, TableRow, TableBody, TableCell, TablePagination, FormGroup, FormControlLabel } from '@material-ui/core'

import CreditCardIcon from '@material-ui/icons/CreditCard'
import HistoryIcon from '@material-ui/icons/History'

import FormWrapper from '../../../components/FormWrapper'
import { createData } from '../../../helpers/BillingHelper'

import { VisaIcon } from '../../../icons/Visa'
import { MasterCardIcon } from '../../../icons/MasterCard'
import { AmexIcon } from '../../../icons/Amex'
import { PayPalIcon } from '../../../icons/PayPal'

import './BillingInformation.less'
import BillingTable from './components/BillingTable'

export interface BillingInformationProps {}

const rows = [createData('1', 100010334, 1.25, false, new Date(2019, 5, 26, 11, 12, 0))]

function BillingInformation() {
    return (
        <React.Fragment>
            <Grid container spacing={3}>
                <Grid item xs={12} lg={4}>
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
                                <a href="#">Update payment method</a>
                            </Grid>
                        </Grid>
                    </Paper>

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
                </Grid>
                <Grid item xs={12} lg={8}>
                    <Paper className="paper wide">
                        <Typography variant="h2" className="override-h2" component="h2">
                            <HistoryIcon /> Billing history
                        </Typography>

                        <FormWrapper>
                            <BillingTable loading={false} error={false} rows={rows} full={true} />
                        </FormWrapper>
                    </Paper>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default BillingInformation

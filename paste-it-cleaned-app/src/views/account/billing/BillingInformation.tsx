import React, { Fragment } from 'react'
import moment from 'moment'
import { TextField, Paper, Grid, Button, Typography, Select, MenuItem, InputLabel, FormControl, Table, TableHead, TableRow, TableBody, TableCell, TablePagination, FormGroup, FormControlLabel } from '@material-ui/core'
import FormWrapper from '../../../components/FormWrapper'
import CreditCardIcon from '@material-ui/icons/CreditCard'
import HistoryIcon from '@material-ui/icons/History'
import PictureAsPdfIcon from '@material-ui/icons/PictureAsPdf'
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline'
import ErrorOutlineIcon from '@material-ui/icons/ErrorOutline'

import { VisaIcon } from './components/Visa'
import { MasterCardIcon } from './components/MasterCard'
import { AmexIcon } from './components/Amex'

import './BillingInformation.less'
import { PayPalIcon } from './components/PayPal'

export interface BillingInformationProps {}

const columns = [
    { id: 'number', label: 'Number', minWidth: 150 },
    { id: 'amount', label: 'Amount', minWidth: 150 },
    { id: 'status', label: 'Status', minWidth: 150 },
    { id: 'paidon', label: 'Paid on', align: 'right', minWidth: 200 },
    { id: 'pdf', label: '', align: 'center', minWidth: 30 },
]

function createData(trxId: string, number: string, amount: number, status: 'paid' | 'pending', paidOn: Date) {
    var numberNode = <a href={`/billing/invoice/${number}`}>#{number}</a>
    var amountNode = amount.toString()
    var statusNode =
        status === 'paid' ? (
            <span className="billing-status-paid">
                <CheckCircleOutlineIcon /> Paid
            </span>
        ) : (
            <span className="billing-status-pending">
                <ErrorOutlineIcon /> Pending
            </span>
        )
    var paidOnNode = moment(paidOn).format('YYYY-MM-DD HH:MM:SS')
    var pdfNode = (
        <a href={`/billing/invoice/${number}`}>
            <PictureAsPdfIcon />
        </a>
    )
    return { key: trxId, number: numberNode, amount: amountNode, status: statusNode, paidon: paidOnNode, pdf: pdfNode }
}

const rows = [createData('1', '100010334', 1.25, 'pending', new Date(2019, 5, 26, 11, 12, 0)), createData('2', '100010335', 2.25, 'paid', new Date(2019, 4, 26, 11, 12, 0))]

function BillingInformation() {
    const [page, setPage] = React.useState(0)
    const [rowsPerPage, setRowsPerPage] = React.useState(10)

    const handleChangePage = (event: any, newPage: any) => {
        setPage(newPage)
    }

    const handleChangeRowsPerPage = (event: any) => {
        setRowsPerPage(+event.target.value)
        setPage(0)
    }

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
                            <div className="table-wrapper">
                                <Table stickyHeader aria-label="sticky table">
                                    <TableHead>
                                        <TableRow>
                                            {columns.map(column => (
                                                <TableCell key={column.id} align={column.align as any} style={{ minWidth: column.minWidth }}>
                                                    {column.label}
                                                </TableCell>
                                            ))}
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {rows.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map(row => {
                                            return (
                                                <TableRow hover tabIndex={-1} key={row.key}>
                                                    {columns.map(column => {
                                                        const value = (row as any)[column.id] as any
                                                        return (
                                                            <TableCell key={column.id} align={column.align as any}>
                                                                {value}
                                                            </TableCell>
                                                        )
                                                    })}
                                                </TableRow>
                                            )
                                        })}
                                    </TableBody>
                                </Table>
                            </div>
                            <TablePagination
                                rowsPerPageOptions={[10, 25, 100]}
                                component="div"
                                count={rows.length}
                                rowsPerPage={rowsPerPage}
                                page={page}
                                backIconButtonProps={{
                                    'aria-label': 'previous page',
                                }}
                                nextIconButtonProps={{
                                    'aria-label': 'next page',
                                }}
                                onChangePage={handleChangePage}
                                onChangeRowsPerPage={handleChangeRowsPerPage}
                            />
                        </FormWrapper>
                    </Paper>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default BillingInformation

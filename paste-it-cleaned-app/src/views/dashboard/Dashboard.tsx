import React, { Fragment } from 'react'

import { ResponsiveContainer, CartesianGrid, XAxis, YAxis, Tooltip, AreaChart, Area } from 'recharts'
import { Grid, Card, Paper, Typography } from '@material-ui/core'

import SettingsIcon from '@material-ui/icons/Settings'
import FileCopyIcon from '@material-ui/icons/FileCopy'
import AssessmentIcon from '@material-ui/icons/Assessment'
import CreditCardIcon from '@material-ui/icons/CreditCard'
import BugReportIcon from '@material-ui/icons/BugReport'

import FormWrapper from '../../components/FormWrapper'
import HitsTable from '../analytics/hits/components/HitsTable'

import { createData } from '../../helpers/AnalyticsHelper'
import { createData as createDataBilling } from '../../helpers/BillingHelper'

import PageWrapper from '../../components/PageWrapper'
import BillingTable from '../account/billing/components/BillingTable'

import './Dashboard.less'

export interface DashboardProps {}

function createDataChart(num: number) {
    return {
        name: num.toString(),
        Pastes: Math.floor(Math.random() * 200),
    }
}

const dataWeb = [
    createDataChart(1),
    createDataChart(2),
    createDataChart(3),
    createDataChart(4),
    createDataChart(5),
    createDataChart(6),
    createDataChart(7),
    createDataChart(8),
    createDataChart(9),
    createDataChart(10),
    createDataChart(11),
    createDataChart(12),
    createDataChart(13),
    createDataChart(14),
    createDataChart(15),
    createDataChart(16),
    createDataChart(17),
    createDataChart(18),
    createDataChart(19),
    createDataChart(20),
    createDataChart(21),
    createDataChart(22),
    createDataChart(23),
    createDataChart(24),
    createDataChart(25),
    createDataChart(26),
    createDataChart(27),
    createDataChart(28),
    createDataChart(29),
    createDataChart(30),
    createDataChart(31),
]

const rows = [
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Excel', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'PowerPoint', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Web', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Text', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Image', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
]

const rowsB = [createDataBilling('1', '100010334', 1.25, 'pending', new Date(2019, 5, 26, 11, 12, 0)), createDataBilling('2', '100010335', 2.25, 'paid', new Date(2019, 4, 26, 11, 12, 0))]

class Dashboard extends React.Component<DashboardProps> {
    render() {
        return (
            <Fragment>
                <div style={{ background: 'linear-gradient(160deg,#2c2c2c,#1f1f1f)' }}>
                    <ResponsiveContainer width="100%" height={400}>
                        <AreaChart data={dataWeb} margin={{ top: 30, right: 50, left: 0, bottom: 10 }}>
                            {this.getDefs()}
                            {this.getCartesianGrid()}
                            {this.getXAxis()}
                            {this.getYAxis()}
                            {this.getToolTip()}
                            <Area type="monotone" dataKey="Pastes" stroke="#fff" fill="url(#colorPastes)" strokeWidth="2px" legendType="circle" />
                        </AreaChart>
                    </ResponsiveContainer>
                </div>
                <div style={{ marginTop: '-10px' }}>
                    <Grid container direction="row" justify="center" alignItems="center" alignContent="center">
                        <Grid item xs={12} sm={6} md={3}>
                            <Card className="dashboard-top-card">
                                <SettingsIcon />
                                Manage plugin settings
                            </Card>
                        </Grid>
                        <Grid item xs={12} sm={6} md={3}>
                            <Card className="dashboard-top-card">
                                <AssessmentIcon />
                                View detailed stats
                            </Card>
                        </Grid>
                        <Grid item xs={12} sm={6} md={3}>
                            <Card className="dashboard-top-card">
                                <CreditCardIcon />
                                Configure billing
                            </Card>
                        </Grid>
                        <Grid item xs={12} sm={6} md={3}>
                            <Card className="dashboard-top-card">
                                <BugReportIcon />
                                Report a bug
                            </Card>
                        </Grid>
                    </Grid>
                </div>
                <PageWrapper>
                    <Grid container spacing={3}>
                        <Grid item xs={12} lg={6}>
                            <Paper className="paper wide">
                                <Typography variant="h2" className="override-h2" component="h2">
                                    <FileCopyIcon /> Last plugin uses
                                </Typography>

                                <FormWrapper>
                                    <HitsTable rows={rows} full={false} />
                                </FormWrapper>
                            </Paper>
                        </Grid>
                        <Grid item xs={12} lg={6}>
                            <Paper className="paper wide">
                                <Typography variant="h2" className="override-h2" component="h2">
                                    <CreditCardIcon /> Last invoices
                                </Typography>

                                <FormWrapper>
                                    <BillingTable rows={rowsB} full={false} />
                                </FormWrapper>
                            </Paper>
                        </Grid>
                    </Grid>
                </PageWrapper>
            </Fragment>
        )
    }

    private getDefs() {
        return (
            <defs>
                <linearGradient id="colorPastes" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#fff" stopOpacity={0.2} />
                    <stop offset="95%" stopColor="#fff" stopOpacity={0} />
                </linearGradient>
            </defs>
        )
    }

    private getCartesianGrid() {
        return <CartesianGrid strokeDasharray="0 0" strokeOpacity={0.3} vertical={false} />
    }

    private getXAxis() {
        return <XAxis axisLine={false} dataKey="name" />
    }

    private getYAxis() {
        return <YAxis axisLine={false} />
    }

    private getToolTip() {
        return (
            <Tooltip
                offset={30}
                contentStyle={{ borderRadius: 10 }}
                itemStyle={{ lineHeight: 0.6, color: '#333' }}
                labelStyle={{ marginBottom: 10, color: '#333', fontWeight: 'bold' }}
                labelFormatter={(label: string | number) => {
                    var theDate = new Date(new Date().getFullYear(), new Date().getMonth(), label as number)
                    return theDate.toLocaleDateString()
                }}
            />
        )
    }
}

export default Dashboard
